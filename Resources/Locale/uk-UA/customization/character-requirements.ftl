character-age-requirement = Ви повинні {$inverted ->
    [true] не бути
    *[other] бути
} бути у межах [color=yellow]{$min}[/color] та [color=yellow]{$max}[/color] років
character-species-requirement = Ви повинні {$inverted ->
    [true] не бути
    *[other] бути
} a [color=green]{$species}[/color]
character-trait-requirement = Ви повинні {$inverted ->
    [true] не мати
    *[other] мати
} ознаки [color=lightblue]{$traits}[/color]
character-backpack-type-requirement = Ви повинні {$inverted ->
    [true] не використовувати
    *[other] використовувати
} a [color=lightblue]{$type}[/color] як ваш мішок
character-clothing-preference-requirement = Ви повинні {$inverted ->
    [true] не носити
    *[other] носити
} a [color=lightblue]{$type}[/color]

character-job-requirement = Ви повинні {$inverted ->
    [true] не бути
    *[other] бути
} одна з цих робіт: {$jobs}
character-department-requirement = Ви повинні {$inverted ->
    [true] не бути
    *[other] бути
} в одному з цих відділів: {$departments}

character-timer-department-insufficient = Вам потрібно [color=yellow]{TOSTRING($time, "0")}[/color] більше хвилин ігрового часу [color={$departmentColor}]{$department}[/color] відділу
character-timer-department-too-high = Вам потрібно [color=yellow]{TOSTRING($time, "0")}[/color] менше хвилин у [color={$departmentColor}]{$department}[/color] відділі
character-timer-overall-insufficient = Вам потрібно [color=yellow]{TOSTRING($time, "0")}[/color] більше хвилин ігрового часу
character-timer-overall-too-high = Вам потрібно [color=yellow]{TOSTRING($time, "0")}[/color] менше хвилин ігрового часу
character-timer-role-insufficient = Вам потрібно [color=yellow]{TOSTRING($time, "0")}[/color] більше хвилин з [color={$departmentColor}]{$job}[/color]
character-timer-role-too-high = Вам знадобиться [color=yellow] {TOSTRING($time, "0")}[/color] менше часу з [color={$departmentColor}]{$job}[/color]

character-trait-group-exclusion-requirement = Якщо ви виберете цю опцію, ви не можете мати одну з наступних рис: {$traits}
character-loadout-group-exclusion-requirement = Якщо ви виберете цей параметр, ви не зможете мати жодного з наступних завантажень: {$loadouts}
