whitelist-not-whitelisted = Ви не у вайтлісті.

# proper handling for having a min/max or not
whitelist-playercount-invalid = {$min ->
    [0] The whitelist for this server only applies below {$max} players.
    *[other] The whitelist for this server only applies above {$min} {$max ->
        [2147483647] -> players, so you may be able to join later.
       *[other] -> players and below {$max} players, so you may be able to join later.
    }
}
whitelist-not-whitelisted-rp = Ви не у вайтлісті. Якщо ви досвідчений гравець, щоб вас додали у вайтліст, зайдіть у наш Дискорд (посилання в лаунчері) та створіть тікет.

command-whitelistadd-description = Додає гравця з зазначеним ніком до вайтліста.
command-whitelistadd-help = whitelistadd <нік>
command-whitelistadd-existing = {$username} вже у вайтлісті!
command-whitelistadd-added = {$username} додано у вайтліст
command-whitelistadd-not-found = Не вийшло знайти '{$username}'

command-whitelistremove-description = Видалити гравця з таким ніком з вайтлісту.
command-whitelistremove-help = whitelistremove <нік>
command-whitelistremove-existing = {$username} не у вайтлісті!
command-whitelistremove-removed = {$username} видалено з вайтліста
command-whitelistremove-not-found = Неможливо знайти '{$username}'

command-kicknonwhitelisted-description = Кікнути всіх гравців не у вайтлісті з сервера.
command-kicknonwhitelisted-help = kicknonwhitelisted

ban-banned-permanent = Цього бану можна позбавитись лише оскарежнням.
ban-banned-permanent-appeal = Цього бану можна позбавитись лише оскарежнням. Ви можете оскаржити на {$link}
ban-expires = Цей бан на {$duration} хвилин і він скінчиться в {$time} UTC (час Лондона).
ban-banned-1 = Ви або інший користувач цього компа або мережі забанені на цьому сервері.
ban-banned-2 = Причина бану: "{$reason}"
ban-banned-3 = Спроба обійти бан, наприклад свторення нового профіля, буде знайдена.

soft-player-cap-full = Сервер повний!
panic-bunker-account-denied = Сервер у режимі бункера для захисту від атаки. Нові підключення не будуть прийняті. Спробуйте пізніше.
panic-bunker-account-denied-reason = Сервер у режимі бункера для захисту від атаки, вас не підключило. Причина: "{$reason}"
panic-bunker-account-reason-account = Профіль має бути старшим за {$minutes} хвилин
panic-bunker-account-reason-overall = Кількість награних годин має бути {$hours} годин
