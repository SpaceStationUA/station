mail-recipient-mismatch = Recipient name or job does not match.
mail-invalid-access = Recipient name and job match, but access isn't as expected.
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

command-mailto-description = Queue a parcel to be delivered to an entity. Example usage: `mailto 1234 5678 false false`. The target container's contents will be transferred to an actual mail parcel.
command-mailto-help = Usage: {$command} <recipient entityUid> <container entityUid> [is-fragile: true or false] [is-priority: true or false]
command-mailto-no-mailreceiver = Target recipient entity does not have a {$requiredComponent}.
command-mailto-no-blankmail = The {$blankMail} prototype doesn't exist. Something is very wrong. Contact a programmer.
command-mailto-bogus-mail = {$blankMail} did not have {$requiredMailComponent}. Something is very wrong. Contact a programmer.
command-mailto-invalid-container = Target container entity does not have a {$requiredContainer} container.
command-mailto-unable-to-receive = Target recipient entity was unable to be setup for receiving mail. ID may be missing.
command-mailto-no-teleporter-found = Target recipient entity was unable to be matched to any station's mail teleporter. Recipient may be off-station.
command-mailto-success = Success! Mail parcel has been queued for next teleport in {$timeToTeleport} seconds.

command-mailnow = Force all mail teleporters to deliver another round of mail as soon as possible. This will not bypass the undelivered mail limit.
command-mailnow-help = Usage: {$command}
command-mailnow-success = Success! All mail teleporters will be delivering another round of mail soon.
