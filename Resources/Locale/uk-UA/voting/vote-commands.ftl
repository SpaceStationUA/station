### Voting system related console commands

## 'createvote' command

cmd-createvote-desc = Створити голосування
cmd-createvote-help = Використовуйте: createvote <'restart'|'preset'|'map'>
cmd-createvote-cannot-call-vote-now = Зараз це зробити не можна!
cmd-createvote-invalid-vote-type = Неправильний тип
cmd-createvote-arg-vote-type = <vote type>

## 'customvote' command

cmd-customvote-desc = Створити своє голосування
cmd-customvote-help = Використовуйте: customvote <title> <option1> <option2> [option3...]
cmd-customvote-on-finished-tie = Нічия між {$ties}!
cmd-customvote-on-finished-win = Переможець: {$winner}!
cmd-customvote-arg-title = <title>
cmd-customvote-arg-option-n = <option{ $n }>

## 'vote' command

cmd-vote-desc = Проголосувати в активному голосуванні
cmd-vote-help = vote <voteId> <option>
cmd-vote-cannot-call-vote-now = Ви не можете зараз викликати голосування!
cmd-vote-on-execute-error-must-be-player = Має бути гравець
cmd-vote-on-execute-error-invalid-vote-id = Недійсний ID голосування
cmd-vote-on-execute-error-invalid-vote-options = Недійсні варіанти для голосування
cmd-vote-on-execute-error-invalid-vote = Недійсне голосування
cmd-vote-on-execute-error-invalid-option = Недійсний варіант

## 'listvotes' command

cmd-listvotes-desc = Списки активних голосувань
cmd-listvotes-help = Використовуйте: listvotes

## 'cancelvote' command

cmd-cancelvote-desc = Скасовує активне голосування
cmd-cancelvote-help = Використовуйте: cancelvote <id>
                      Ви можете отримати ID голосування за допомогою команди: listvotes.
cmd-cancelvote-error-invalid-vote-id = Недійсний ID голосування
cmd-cancelvote-error-missing-vote-id = Відсутній ID
cmd-cancelvote-arg-id = <id>
