mail-recipient-mismatch = Ім'я або посада отримувача не збігаються.
mail-invalid-access = Ім'я та посада отримувача збігаються, але доступ не відповідає очікуваному.
mail-locked = Захисний замок ще не знято. Прикладіть ID отримувача.
mail-desc-far = Поштова посилка. Ви не можете розібрати, кому вона адресована, на такій відстані.
mail-desc-close = Поштова посилка, адресована {CAPITALIZE($name)}, {$job}.
mail-desc-fragile = Вона має [color=red]червону наклейку "крихке"[/color].
mail-desc-priority = На захисному замку активована [color=yellow]жовта стрічка пріоритету[/color]. Краще доставити її вчасно!
mail-desc-priority-inactive = На захисному замку неактивна [color=#886600]жовта стрічка пріоритету[/color].
mail-unlocked = Захисну систему розблоковано.
mail-unlocked-by-emag = Захисна система *BZZT*.
mail-unlocked-reward = Захисну систему розблоковано. На рахунок логістики додано {$bounty} спесо.
mail-penalty-lock = ЗЛАМАНО ЗАХИСНИЙ ЗАМОК. РАХУНОК ЛОГІСТИКИ ШТРАФОВАНО НА {$credits} СПЕСО.
mail-penalty-fragile = ЦІЛІСНІСТЬ ПОРУШЕНО. РАХУНОК ЛОГІСТИКИ ШТРАФОВАНО НА {$credits} СПЕСО.
mail-penalty-expired = ТЕРМІН ДОСТАВКИ ПРОЙШОВ. РАХУНОК ЛОГІСТИКИ ШТРАФОВАНО НА {$credits} СПЕСО.
mail-item-name-unaddressed = пошта
mail-item-name-addressed = пошта ({$recipient})

command-mailto-description = Черга посилок для доставки об'єкту. Приклад використання: `mailto 1234 5678 false false`. Вміст цільового контейнера буде перенесено в реальну поштову посилку.
command-mailto-help = Використання: {$command} <отримувач entityUid> <контейнер entityUid> [крихке: true або false] [пріоритетне: true або false] [велике: true або false, необов'язково]
command-mailto-no-mailreceiver = Цільовий отримувач не має {$requiredComponent}.
command-mailto-no-blankmail = Прототип {$blankMail} не існує. Щось дуже не так. Зверніться до програміста.
command-mailto-bogus-mail = {$blankMail} не мав {$requiredMailComponent}. Щось дуже не так. Зверніться до програміста.
command-mailto-invalid-container = Цільовий контейнер не має потрібного контейнера {$requiredContainer}.
command-mailto-unable-to-receive = Цільовий отримувач не може приймати пошту. Може бути відсутній ID.
command-mailto-no-teleporter-found = Цільовий отримувач не відповідає жодному поштовому телепортеру станції. Можливо, отримувач знаходиться за межами станції.
command-mailto-success = Успіх! Поштова посилка поставлена в чергу на телепортацію через {$timeToTeleport} секунд.

# Mailnow

command-mailnow = Примусово змусити всі поштові телепортери здійснити наступну доставку пошти якнайшвидше. Це не перевищить ліміт недоставленої пошти.
command-mailnow-help = Використання: {$command}
command-mailnow-success = Успіх! Усі поштові телепортери незабаром здійснять чергову доставку пошти.

# Mailtestbulk
