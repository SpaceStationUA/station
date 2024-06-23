mail-recipient-mismatch = Ім'я або посада одержувача не збігаються.
mail-invalid-access = Ім'я отримувача та посада збігаються, але доступ не такий, як очікувалося.
mail-locked = Блюспейс-захисна стрічка не прибрана. Проведіть по ній планшетом отримувача.
mail-desc-far = Листівка. Ви не бачите кому вона призначена з цієї відстані.
mail-desc-close = Листівка адресована до {CAPITALIZE($name)}, {$job}.
mail-desc-fragile = На конверті [color=red]червона стрічка[/color].
mail-desc-priority = Блюспейс-захисна стрічка має [color=yellow]жовтий пріоритетний[/color] статус. Краще її вчасно доставити!
mail-desc-priority-inactive = Блюспейс-захисна стрічка [color=#886600]жовтого кольору[/color] вже не активна - прострочена.
mail-unlocked = Блюспейс-захисну систему розблоковано.
mail-unlocked-by-emag = Блюспейс-захисна система видає *БДЗИНЬ*.
mail-unlocked-reward = Блюспейс-захисну систему розблоковано. {$bounty} $ додано на рахунок карго.
mail-penalty-lock = БЛЮСПЕЙС-СТРІЧКУ ПОШКОДЖЕНО. КАРГО ОШТРАФОВАНО НА {$credits} $.
mail-penalty-fragile = ВМІСТЬ РОЗБИТО. КАРГО ОШТРАФОВАНО НА {$credits} $.
mail-penalty-expired = ЗАПІЗНІЛА ДОСТАВКА. КАРГО ОШТРАФОВАНО НА {$credits}.
mail-item-name-unaddressed = листівка
mail-item-name-addressed = листівка для ({$recipient})

command-mailto-description = Поставити посилку в чергу на доставку до організації. Приклад використання: `mailto 1234 5678 false false false`. Вміст цільового контейнера буде передано до реальної поштової посилки.
command-mailto-help = Використання: {$command} <Uid сутності-одержувача> <Uid сутності-контейнера> [is-fragile: true or false] [is-priority: true or false] [is-fragile: true or false] [is-priority: true or false]
command-mailto-no-mailreceiver = Цільовий отримувач не має {$requiredComponent}.
command-mailto-no-blankmail = Прототипу {$blankMail} не існує. Щось дуже неправильно. Зверніться до програміста.
command-mailto-bogus-mail = У {$blankMail} не було {$requiredMailComponent}. Щось дуже неправильно. Зверніться до програміста.
command-mailto-invalid-container = Цільовий контейнер не має контейнера {$requiredContainer}.
command-mailto-unable-to-receive = Не вдалося налаштувати цільового отримувача на отримання пошти. Можливо, відсутній ідентифікатор.
command-mailto-no-teleporter-found = Не вдалося зіставити цільового одержувача з жодним поштовим телепортом станції. Одержувач може перебувати за межами станції.
command-mailto-success = Успішно! Поштову посилку поставлено в чергу на наступний телепорт через {$timeToTeleport} секунд.

command-mailnow = Змусити всі поштові телепорти доставити чергову порцію пошти якнайшвидше. Це не дозволить обійти ліміт недоставленої пошти.
command-mailnow-help = Використання: {$command}
command-mailnow-success = Успіх! Всі поштові телепорти незабаром доставлять чергову порцію пошти.
