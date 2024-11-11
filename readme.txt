Работа ведется через публичный репозитарий
https://github.com/aifeigin/AsiaServe

* Как начать ?

1) установить Windows x64 Git shell extension
https://tortoisegit.org/download/

2) по правой кнопке мыши на папке, где будет размещаться сайт выбрать 
Tortoise Git\Clone

https://github.com/aifeigin/AsiaServe

3) сайт создается в папке AsiaServe, где надо делать дальнейшие изменения

* Как забрать изменения ?

по правой кнопке мыши на папке AsiaServe выбрать Tortoise Git\Pull

* Как опубликовать изменения ?

** Единоразово 
0.1) создать developer token через
https://github.com/settings/profile

<> Developer settings

и скопировать его (например) в файл Eol

0.2) установить Git username email
- найти свой no-replay email address через
https://github.com/settings/emails

- по правой кнопе на папке AsiaServe выбрать Tortoise Git\Settings\Git

- установить User Info 
для источника << Global
-- Name 
-- Email = no-replay email address
сохранить

** Каждый раз

1) для commit изменений
Tortoise Git\Commit (ввести комментарий, желательно осмысленно так как они видны публично)

2) для добавления новых файлов

по правой кнопке мыши на папке AsiaServe

Show More Options
Tortoise Git\Add

3) Для публикации изменений
Tortoise Git\Push

(при запросе единоразово ввести свой Developer Token)

