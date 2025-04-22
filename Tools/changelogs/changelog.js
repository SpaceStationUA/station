// Dependencies
const fs = require("fs");
const yaml = require("js-yaml");
const axios = require("axios");

// Use GitHub token if available
if (process.env.GITHUB_TOKEN) axios.defaults.headers.common["Authorization"] = `Bearer ${process.env.GITHUB_TOKEN}`;

// Regexes
const HeaderRegex = /^\s*(?::cl:|🆑) *([a-z0-9_\- ]+)?\s+/im; // :cl: or 🆑 [0] followed by optional author name [1]
const EntryRegex = /^ *[*-]? *(add|remove|tweak|fix): *([^\n\r]+)\r?$/img; // * or - followed by change type [0] and change message [1]
const CommentRegex = /<!--.*?-->/gs; // HTML comments

// Main function
async function main() {
    // Get PR details
    const pr = await axios.get(`https://api.github.com/repos/${process.env.GITHUB_REPOSITORY}/pulls/${process.env.PR_NUMBER}`);
    const { merged_at, body, user } = pr.data;

    // Remove comments from the body
    commentlessBody = body.replace(CommentRegex, '');

    // Get author
    const headerMatch = HeaderRegex.exec(commentlessBody);
    if (!headerMatch) {
        console.log("No changelog entry found, skipping");
        return;
    }

    let author = headerMatch[1];
    if (!author) {
        console.log("No author found, setting it to author of the PR\n");
        author = user.login;
    }

    // Get all changes from the body
    const entries = getChanges(commentlessBody);


    // Time is something like 2021-08-29T20:00:00Z
    // Time should be something like 2023-02-18T00:00:00.0000000+00:00
    let time = merged_at;
    if (time)
    {
        time = time.replace("z", ".0000000+00:00").replace("Z", ".0000000+00:00");
    }
    else
    {
        console.log("Pull request was not merged, skipping");
        return;
    }


    // Construct changelog yml entry
    const entry = {
        author: author,
        changes: entries,
        id: getHighestCLNumber() + 1,
        time: time,
        url: `https://github.com/${process.env.GITHUB_REPOSITORY}/pull/${process.env.PR_NUMBER}`
    };

    // Write changelogs
    writeChangelog(entry);

    console.log(`Changelog updated with changes from PR #${process.env.PR_NUMBER}`);
}


// Code chunking

// Get all changes from the PR body
function getChanges(body) {
    const matches = [];
    const entries = [];

    for (const match of body.matchAll(EntryRegex)) {
        matches.push([match[1], match[2]]);
    }

    if (!matches)
    {
        console.log("No changes found, skipping");
        return;
    }


    // Check change types and construct changelog entry
    matches.forEach((entry) => {
        let type;

        switch (entry[0].toLowerCase()) {
            case "add":
                type = "Add";
                break;
            case "remove":
                type = "Remove";
                break;
            case "tweak":
                type = "Tweak";
                break;
            case "fix":
                type = "Fix";
                break;
            default:
                break;
        }

        if (type) {
            entries.push({
                type: type,
                message: entry[1],
            });
        }
    });

    return entries;
}

// Get the highest changelog number from the changelogs file
function getHighestCLNumber() {
    const filePath = `../../${process.env.CHANGELOG_DIR}`;
    let highestId = 0;

    if (fs.existsSync(filePath)) {
        try {
            const file = fs.readFileSync(filePath, "utf8");
            const data = yaml.load(file);
            if (data && Array.isArray(data.Entries)) {
                 const clNumbers = data.Entries.map((entry) => entry.id).filter(id => typeof id === 'number');
                 if (clNumbers.length > 0) {
                    highestId = Math.max(...clNumbers);
                 }
            }
        } catch (e) {
            console.error(`Error reading or parsing ${filePath}:`, e);
        }
    }

    return highestId;
}

function writeChangelog(entry) {
    const filePath = `../../${process.env.CHANGELOG_DIR}`;
    let data = {};

    if (fs.existsSync(filePath)) {
        try {
            const file = fs.readFileSync(filePath, "utf8");
            data = yaml.load(file);
            if (typeof data !== 'object' || data === null) {
                console.warn(`Warning: ${filePath} is malformed. Initializing with default structure.`);
                data = {};
            }
        } catch (e) {
            console.error(`Error reading or parsing ${filePath}. Initializing with default structure.`, e);
            data = {};
        }
    } else {
        console.log(`${filePath} not found. Creating new file with default structure.`);
        data = {
            Name: "Pirate",
            AdminOnly: false,
            Order: 3,
            Entries: []
        };
    }

    if (!Array.isArray(data.Entries)) {
        console.warn(`Warning: 'Entries' key in ${filePath} is not an array or is missing. Initializing it.`);
        data.Entries = [];
    }

    data.Entries.push(entry);

    try {
        fs.writeFileSync(
            filePath,
            yaml.dump(data, { indent: 2 })
        );
    } catch (e) {
        console.error(`Error writing updated changelog to ${filePath}:`, e);
    }
}

// Run main
main();
