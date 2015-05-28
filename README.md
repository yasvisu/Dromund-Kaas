# Dromund-Kaas
Dromund Kaas team - C# game for the Advanced C# course

# Star Wars: The Sith Invader Game.

### The Game:

* You are the Space Commander!
  * Choose your ship.
  * Rescue the Galaxy!

Included:
* TWO WHOLE LEVELS!
* More than 5 Commander ship styles.
* Various Enemy Ship styles.
* A charming adjutant to speak out when it matters the most.

### Technical Features:

* Easily extendable code:
  * Fully documented
  * Neatly formatted.
* The capability to add / remove levels, enemies, bosses, and modify their sprites without touching the source code.
* Reusable and extendable classes.
* Great to tinker and polish.

---

#### Как се наглася Git (cmd версия):

* Инсталирате Гит - http://git-scm.com/ - тази стъпка е нужна независимо дали ще използвате CMD или TortoiseGit

0. Конфигурирате Гит на машината си (в cmd):

      `git config --global user.name "YOUR NAME"`
    
     `git config --global user.email "YOUR EMAIL ADDRESS"`
     
1. Отивате в директорията, където ще си правите локална репозитория.

     `cd C:\baba\`
     Ако е на друг драйв, пишете първо `D:`, за да се преместите на него. След това - `cd ...`.
     
2. Инициализирате Гит

     `git init` => създава ви се скрита .git папка.
     
3. Казвате "това място е за онова място":

     `git remote add origin ---url---` - ще наричаме origin отдалечената репозитория; URL заменяте с clone url-a, който Гитхъб ви дава.
     
Готови сте!

=> Как се работи с Гит?

Интересуват ви 4-на команди:

1. `git pull -u origin master` - с други думи, `git pull` от origin в master. (или дърпаме нещата при нас)
2. `git add .` - прибавяме всички променени файлове в нов комит, готов за пращане. Ако искаме специфични да прибавим, пишем ги поименно вместо точката.
3. `git commit -m "blablabla"` - приготвяме комит (принос към репозиторията в Гитхъб) със съобщение "блабла". Важно: не пишете "блабла", ами конкретно какво променяте!
4. `git push` - качвате всичко в главната репозитория. Ако тя е на по-нова версия от вас, трябва първо да направите `git pull ...` - и ако има конфликти, трябва да се справите с тях. Как става това - четете.
5. `git status` - когато се чудите какво става с гит. Чисто информативно.

И това е!
пушвайте ;)
