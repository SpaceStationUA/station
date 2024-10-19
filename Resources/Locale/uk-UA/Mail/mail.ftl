mail-recipient-mismatch = Ім'я або посада одержувача не збігаються.
mail-invalid-access = Ім'я отримувача та посада збігаються, але доступ не такий, як очікувалося.
mail-locked = Захист від несанкціонованого доступу не знято. Торкніться ідентифікатора одержувача.
mail-desc-far = Поштова посилка. З такої відстані не розбереш, кому вона адресована.
mail-desc-close = Поштова посилка на адресу {CAPITALIZE($name)}, {$job}.
mail-desc-fragile = Він має [color=red]червону тендітну етикетку[/color].
mail-desc-priority = Активна [color=yellow]жовта[/color] стрічка пріоритету антитамперного замка. Краще доставити його вчасно!
mail-desc-priority-inactive = Жовта пріоритетна стрічка [color=#886600]антитамперного замка[/color] неактивна.
mail-unlocked = Система захисту від несанкціонованого доступу розблокована.
mail-unlocked-by-emag = Система захисту від несанкціонованого доступу *БДЗИНЬ*.
mail-unlocked-reward = Систему захисту від несанкціонованого доступу розблоковано. На рахунок логіста додано {$bounty} спесо.
mail-penalty-lock = ЗЛАМАНО ПРОТИЗЛАМНИЙ ЗАМОК. Рахунок ЛОГІСТИК БАНКу ОШТРАФОВАНО на {$credits}. СПЕСОС.
mail-penalty-fragile = ЦІЛІСНІСТЬ СКОМПРОМЕТОВАНА. Рахунок ЛОГІСТИК БАНКу ОШТРАФОВАНО на {$credits} СПЕСОС.
mail-penalty-expired = ПОСТАВКА ПРОСТРОЧЕНА. Рахунок ЛОГІСТИК БАНКу ОШТРАФОВАНО на {$credits} СПЕСОС.
mail-item-name-unaddressed = пошта
mail-item-name-addressed = листівка для ({$recipient})

command-mailto-description = Поставити посилку в чергу на доставку до організації. Приклад використання: `mailto 1234 5678 false false false`. Вміст цільового контейнера буде передано до реальної поштової посилки.
command-mailto-help = Використання: {$command} <Uid сутності-одержувача> <Uid сутності-контейнера> [is-fragile: true або false] [is-priority: true або false] [is-large: true або false, необов'язково]
command-mailto-no-mailreceiver = Цільовий отримувач не має {$requiredComponent}.
command-mailto-no-blankmail = Прототипу {$blankMail} не існує. Щось дуже неправильно. Зверніться до програміста.
command-mailto-bogus-mail = У {$blankMail} не було {$requiredMailComponent}. Щось дуже неправильно. Зверніться до програміста.
command-mailto-invalid-container = Цільовий контейнер не має контейнера {$requiredContainer}.
command-mailto-unable-to-receive = Не вдалося налаштувати цільового отримувача на отримання пошти. Можливо, відсутній ідентифікатор.
command-mailto-no-teleporter-found = Не вдалося зіставити цільового одержувача з жодним поштовим телепортом станції. Одержувач може перебувати за межами станції.
command-mailto-success = Успішно! Поштову посилку поставлено в чергу на наступний телепорт через {$timeToTeleport} секунд.

# Mailnow

command-mailnow = Змусити всі поштові телепорти доставити чергову порцію пошти якнайшвидше. Це не дозволить обійти ліміт недоставленої пошти.
command-mailnow-help = Використання: {$command}
command-mailnow-success = Успіх! Всі поштові телепорти незабаром доставлять чергову порцію пошти.

# Mailtestbulk
