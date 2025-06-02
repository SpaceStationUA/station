#!/usr/bin/env python3

#
# Sends updates to a Discord webhook for new changelog entries since the last GitHub Actions publish run.
# Automatically figures out the last run and changelog contents with the GitHub API.
#

import io
import itertools
import os
import requests
import yaml
from typing import Any, Iterable

GITHUB_API_URL    = os.environ.get("GITHUB_API_URL", "https://api.github.com")
GITHUB_REPOSITORY = os.environ["GITHUB_REPOSITORY"]
GITHUB_RUN        = os.environ["GITHUB_RUN_ID"]
GITHUB_TOKEN      = os.environ["GITHUB_TOKEN"]
CHANGELOG_DIR     = os.environ["CHANGELOG_DIR"]
CHANGELOG_WEBHOOK = os.environ["CHANGELOG_WEBHOOK"]

# https://discord.com/developers/docs/resources/webhook
DISCORD_SPLIT_LIMIT = 2000

TYPES_TO_EMOJI = {
    "Fix":    "🐛",
    "Add":    "✨",
    "Remove": "❌",
    "Tweak":  "⚒️"
}

ChangelogEntry = dict[str, Any]

def main():
    if not CHANGELOG_WEBHOOK:
        return

    session = requests.Session()
    session.headers["Authorization"]        = f"Bearer {GITHUB_TOKEN}"
    session.headers["Accept"]               = "Accept: application/vnd.github+json"
    session.headers["X-GitHub-Api-Version"] = "2022-11-28"

    most_recent = get_most_recent_workflow(session)
    last_sha = most_recent['head_commit']['id']
    print(f"Last successful publish job was {most_recent['id']}: {last_sha}")
    last_changelog = yaml.safe_load(get_last_changelog(session, last_sha))
    with open(CHANGELOG_DIR, "r") as f:
        cur_changelog = yaml.safe_load(f)

    diff = diff_changelog(last_changelog, cur_changelog)
    send_to_discord(diff)


def get_most_recent_workflow(sess: requests.Session) -> Any:
    workflow_run = get_current_run(sess)
    past_runs = get_past_runs(sess, workflow_run)
    for run in past_runs['workflow_runs']:
        # First past successful run that isn't our current run.
        if run["id"] == workflow_run["id"]:
            continue

        return run


def get_current_run(sess: requests.Session) -> Any:
    resp = sess.get(f"{GITHUB_API_URL}/repos/{GITHUB_REPOSITORY}/actions/runs/{GITHUB_RUN}")
    resp.raise_for_status()
    return resp.json()


def get_past_runs(sess: requests.Session, current_run: Any) -> Any:
    """
    Get all successful workflow runs before our current one.
    """
    params = {
        "status": "success",
        "created": f"<={current_run['created_at']}"
    }
    resp = sess.get(f"{current_run['workflow_url']}/runs", params=params)
    resp.raise_for_status()
    return resp.json()


def get_last_changelog(sess: requests.Session, sha: str) -> str:
    """
    Use GitHub API to get the previous version of the changelog YAML (Actions builds are fetched with a shallow clone)
    """
    params = {
        "ref": sha,
    }
    headers = {
        "Accept": "application/vnd.github.raw"
    }

    resp = sess.get(f"{GITHUB_API_URL}/repos/{GITHUB_REPOSITORY}/contents/{CHANGELOG_DIR}", headers=headers, params=params)
    resp.raise_for_status()
    return resp.text


def diff_changelog(old: dict[str, Any], cur: dict[str, Any]) -> Iterable[ChangelogEntry]:
    """
    Find all new entries not present in the previous publish.
    """
    old_entry_ids = {e["id"] for e in old["Entries"]}
    return (e for e in cur["Entries"] if e["id"] not in old_entry_ids)


def get_discord_body(content: str, author_nickname: str):
    return {
        "content": content,
        # Do not allow any mentions.
        "allowed_mentions": {
            "parse": []
        },
    }


def send_discord(content: str, author_nickname: str):
    body = get_discord_body(content, author_nickname)

    response = requests.post(CHANGELOG_WEBHOOK, json=body)
    print(f"Discord API Response Status: {response.status_code}")
    print(f"Discord API Response Text: {response.text}")
    try:
        print(f"Discord API Response JSON: {response.json()}")
    except requests.exceptions.JSONDecodeError:
        print("Discord API Response was not valid JSON.")
    response.raise_for_status()


def send_to_discord(entries: Iterable[ChangelogEntry]) -> None:
    if not CHANGELOG_WEBHOOK:
        print(f"No discord webhook URL found, skipping discord send")
        return

    for name, group in itertools.groupby(entries, lambda x: x["author"]):
        author_lines = []
        author_lines.append(f"**{name}**:\n")

        for entry in group:
            for change_item in entry["changes"]:
                emoji = TYPES_TO_EMOJI.get(change_item['type'], "❓")
                message = change_item['message']
                entry_specific_url = entry.get("url")

                line = ""
                if entry_specific_url and entry_specific_url.strip():
                    line = f"{emoji} - [{message}](<{entry_specific_url}>)\n"
                else:
                    line = f"{emoji} - {message}\n"
                author_lines.append(line)

        if len(author_lines) == 1:
            print(f"No changes to report for {name}, skipping.")
            continue

        support_link_markdown = f"\n\n💰 [Підтримати {name}](https://donatello.to/SpaceStationUA?a=100&c=Підтримка%20Кодера&m=Для%20Кодера:{name})\n"
        author_lines.append(support_link_markdown)

        message_part_buffer = io.StringIO()
        for i, line_to_add in enumerate(author_lines):
            if len(message_part_buffer.getvalue()) + len(line_to_add) >= DISCORD_SPLIT_LIMIT:
                content_to_send = message_part_buffer.getvalue()
                if content_to_send.strip():
                    print(f"Sending split part of message for {name}")
                    send_discord(content_to_send, name)
                message_part_buffer = io.StringIO()
            message_part_buffer.write(line_to_add)
        final_content_part = message_part_buffer.getvalue()
        if final_content_part.strip(): # Ensure not sending an empty final part
            print(f"Sending final message part for {name}")
            send_discord(final_content_part, name)


if __name__ == "__main__":
    main()
