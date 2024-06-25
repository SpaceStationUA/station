-create-3rd-person = { $chance ->
        [1] створює
        *[other] створює
    }

-cause-3rd-person = { $chance ->
        [1] Причини
        *[other] причина
    }

-satiate-3rd-person = { $chance ->
        [1] насичує
        *[other] насичує
    }

reagent-effect-guidebook-create-entity-reaction-effect = { $chance ->
        [1] створює
        *[other] створює
    } { $amount ->
        [1] {INDEFINITE($entname)}
        *[other] {$amount} {MAKEPLURAL($entname)}
    }

reagent-effect-guidebook-explosion-reaction-effect = { $chance ->
        [1] Причини
        *[other] причина
    } вибух

reagent-effect-guidebook-emp-reaction-effect = { $chance ->
        [1] Причини
        *[other] причина
    } електромагнітний імпульс

reagent-effect-guidebook-foam-area-reaction-effect =
    { $chance ->
        [1] Creates
        *[other] create
    } large quantities of foam

reagent-effect-guidebook-foam-area-reaction-effect = { $chance ->
        [1] створює
        *[other] створює
    } велику кількість диму

reagent-effect-guidebook-satiate-thirst = { $chance ->
        [1] насичує
        *[other] насичує
    } { $relative ->
        [1] спрагу в середньому
        *[other] втамовує спрагу з {NATURALFIXED($relative, 3)}x середньою швидкістю
    }

reagent-effect-guidebook-satiate-hunger = { $chance ->
        [1] насичує
        *[other] насичує
    } { $relative ->
        [1] голод в середньому
        *[other] голод з {NATURALFIXED($relative, 3)}x середньою швидкістю
    }

reagent-effect-guidebook-health-change = { $chance ->
        [1] { $healsordeals ->
                [heals] Зцілює
                [deals] Укладає угоди
                *[both] Змінює здоров'я на
             }
        *[other] { $healsordeals ->
                    [heals] зцілює
                    [deals] укладає угоду
                    *[both] змінюють здоров'я на
                 }
    } { $changes }

reagent-effect-guidebook-status-effect = { $type ->
        [add]   { $chance ->
                    [1] Causes
                    *[other] cause
                } {LOC($key)} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)} with accumulation
        *[set]  { $chance ->
                    [1] Causes
                    *[other] cause
                } {LOC($key)} for at least {NATURALFIXED($time, 3)} {MANY("second", $time)} without accumulation
        [remove]{ $chance ->
                    [1] Removes
                    *[other] remove
                } {NATURALFIXED($time, 3)} {MANY("second", $time)} of {LOC($key)}
    }

reagent-effect-guidebook-activate-artifact = { $chance ->
        [1] Спроби
        *[other] спроба
    } активувати артефакт

reagent-effect-guidebook-set-solution-temperature-effect = { $chance ->
        [1] Множини
        *[other] встановлює
    } температуру розв'язку з точністю {NATURALFIXED($temperature, 2)}k

reagent-effect-guidebook-adjust-solution-temperature-effect = { $chance ->
        [1] { $deltasign ->
                [1] Додає
                *[-1] Видаляє
            }
        *[other]
            { $deltasign ->
                [1] додає
                *[-1] видалити
            }
    } тепло від розчину, поки воно не досягне { $deltasign ->
                [1] не більше {NATURALFIXED($maxtemp, 2)}k
                *[-1] не менше {NATURALFIXED($mintemp, 2)}k
            }

reagent-effect-guidebook-adjust-reagent-reagent = { $chance ->
        [1] { $deltasign ->
                [1] Додає
                *[-1] Видаляє
            }
        *[other]
            { $deltasign ->
                [1] додає
                *[-1] видалити
            }
    } {NATURALFIXED($amount, 2)}u з {$reagent} { $deltasign ->
        [1] до
        *[-1] від
    } розчин

reagent-effect-guidebook-adjust-reagent-group = { $chance ->
        [1] { $deltasign ->
                [1] Додає
                *[-1] Видаляє
            }
        *[other]
            { $deltasign ->
                [1] додає
                *[-1] видалити
            }
    } {NATURALFIXED($amount, 2)}u реагентів у групі {$group} { $deltasign ->
            [1] до
            *[-1] від
        } розв'язок

reagent-effect-guidebook-adjust-temperature = { $chance ->
        [1] { $deltasign ->
                [1] Додає
                *[-1] Видаляє
            }
        *[other]
            { $deltasign ->
                [1] додає
                *[-1] видалити
            }
    } {POWERJOULES($amount)} тепла { $deltasign ->
            [1] до
            *[-1] від
        }
        від тіла, в якому вона перебуває

reagent-effect-guidebook-chem-cause-disease = { $chance ->
        [1] Причини
        *[other] причина
    } хвороба { $disease }

reagent-effect-guidebook-chem-cause-random-disease = { $chance ->
        [1] Причини
        *[other] причина
    } хвороби { $diseases }

reagent-effect-guidebook-jittering = { $chance ->
        [1] Причини
        *[other] причина
    } тремтіння

reagent-effect-guidebook-chem-clean-bloodstream = { $chance ->
        [1] очищає
        *[other] очищає
    } кровотік від інших хімічних речовин

reagent-effect-guidebook-cure-disease = { $chance ->
        [1] Ліки
        *[other] лікування
    } хвороби

reagent-effect-guidebook-cure-eye-damage = { $chance ->
        [1] { $deltasign ->
                [1] Укладає угоди
                *[-1] Зцілює
            }
        *[other]
            { $deltasign ->
                [1] угода
                *[-1] зцілює
            }
    } пошкодження очей

reagent-effect-guidebook-chem-vomit = { $chance ->
        [1] Причини
        *[other] причина
    } блювота

reagent-effect-guidebook-create-gas = { $chance ->
        [1] створює
        *[other] створює
    } { $moles } { $moles ->
        [1] моль
        *[other] молі
    } of { $gas }

reagent-effect-guidebook-drunk = { $chance ->
        [1] Причини
        *[other] причина
    } пияцтво

reagent-effect-guidebook-electrocute = { $chance ->
        [1] Електрошок
        *[other] удар струмом
    } метаболізатор для {NATURALFIXED($time, 3)} {MANY("second", $time)}

reagent-effect-guidebook-extinguish-reaction = { $chance ->
        [1] гасить
        *[other] гасить
    } вогонь

reagent-effect-guidebook-flammable-reaction = { $chance ->
        [1] Збільшує
        *[other] збільшення
    } займистість

reagent-effect-guidebook-ignite = { $chance ->
        [1] запалює
        *[other] підпалює
    } метаболізатор

reagent-effect-guidebook-make-sentient = { $chance ->
        [1] робить
        *[other] робить
    } метаболізатор розумним

reagent-effect-guidebook-make-polymorph = { $chance ->
        [1] Polymorphs
        *[other] polymorph
    } the metabolizer into a { $entityname }

reagent-effect-guidebook-modify-bleed-amount = { $chance ->
        [1] { $deltasign ->
                [1] Індукує
                *[-1] Зменшує
            }
        *[other] { $deltasign ->
                    [1] індукує
                    *[-1] зменшує
                 }
    } кровотеча

reagent-effect-guidebook-modify-blood-level = { $chance ->
        [1] { $deltasign ->
                [1] Збільшує
                *[-1] Зменшує
            }
        *[other] { $deltasign ->
                    [1] збільшує
                    *[-1] зменшує
                 }
    } рівень крові

reagent-effect-guidebook-paralyze = { $chance ->
        [1] паралізує
        *[other] паралізує
    } метаболізатор принаймні на {NATURALFIXED($time, 3)} {MANY("second", $time)}

reagent-effect-guidebook-movespeed-modifier = { $chance ->
        [1] Змінює
        *[other] змінює
    } швидкість руху на {NATURALFIXED($walkspeed, 3)}x принаймні на {NATURALFIXED($time, 3)} {MANY("second", $time)}

reagent-effect-guidebook-reset-narcolepsy = { $chance ->
        [1] Тимчасово зберігається
        *[other] тимчасово зупиняє
    } від нарколепсії

reagent-effect-guidebook-wash-cream-pie-reaction = { $chance ->
        [1] змиває
        *[other] змиває
    } кремовий пиріг з обличчя

reagent-effect-guidebook-cure-zombie-infection = { $chance ->
        [1] Лікує
        *[other] лікує
    } поточну зомбі-інфекцію

reagent-effect-guidebook-cause-zombie-infection = { $chance ->
        [1] дає
        *[other] дає
    } особі зомбі-інфекцію

reagent-effect-guidebook-innoculate-zombie-infection = { $chance ->
        [1] Ліки
        *[other] лікування
    } поточну зомбі-інфекцію та забезпечує імунітет до майбутніх інфекцій

reagent-effect-guidebook-missing = { $chance ->
        [1] Причини
        *[other] причина
    } невідомий ефект, оскільки його ще ніхто не описав
