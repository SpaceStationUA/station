command-list-langs-desc = Перелічіть мови, якими може розмовляти ваша поточна організація на даний момент.
command-list-langs-help = Usage: {$command}

command-saylang-desc = Send a message in a specific language.
command-saylang-help = Usage: {$command} <language id> <message>. Example: {$command} GalacticCommon "Hello World!"

command-language-select-desc = Select the currently spoken language of your entity.
command-language-select-help = Usage: {$command} <language id>. Example: {$command} GalacticCommon

command-language-spoken = Домовилися:
command-language-understood = Зрозуміло:
command-language-current-entry = {$id}. {$language} - {$name} (поточна)
command-language-entry = {$id}. {$language} - {$name}
command-language-invalid-number = Номер мови має бути від 0 до {$total}. Або використовуйте назву мови.
command-language-invalid-language = Мови {$id} не існує або ви не можете нею розмовляти
command-description-language-add = Додає нову мову до конвеєрного об'єкта. Два останні аргументи вказують на те, чи потрібно розмовляти нею/розуміти її. Приклад: 'self language:add "Canilunzt" true true'
command-description-language-rm = Видаляє мову з конвеєрного об'єкта. Працює подібно до language:add. Приклад: 'self language:rm "GalacticCommon" true true'.
command-description-language-lsspoken = Перераховує всі мови, якими може розмовляти сутність. Приклад: 'self language:lsspoken'
command-description-language-lsunderstood = Перераховує всі мови, які сутність може розуміти. Приклад: 'self language:lssunderstood'
command-description-translator-addlang = Додає нову цільову мову до сутності конвеєрного перекладача. Докладні відомості див. у розділі language:add.
command-description-translator-rmlang = Видаляє цільову мову з конвеєрного перекладача. Докладні відомості наведено у розділі language:rm.
command-description-translator-addrequired = Додає нову необхідну мову до сутності конвеєрного перекладача. Приклад: 'ent 1234 translator:addrequired "GalacticCommon"'
command-description-translator-rmrequired = Вилучає необхідну мову з сутності конвеєрного перекладача. Приклад: 'ent 1234 translator:rmrequired "GalacticCommon"'
command-description-translator-lsspoken = Перераховує всі розмовні мови для сутності конвеєрного перекладача. Приклад: 'ent 1234 translator:lsspoken'
command-description-translator-lsunderstood = Перераховує всі зрозумілі мови для сутності конвеєрного перекладача. Приклад: 'ent 1234 translator:lssunderstood'
command-description-translator-lsrequired = Перераховує всі необхідні мови для сутності конвеєрного перекладача. Приклад: 'ent 1234 translator:lsrequired'
command-language-error-this-will-not-work = Це не спрацює.
command-language-error-not-a-translator = Сутність {$entity} не є перекладачем.
