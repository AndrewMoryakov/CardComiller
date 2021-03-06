# Программа для генерации отчётов на основе данных в файлах *.csv и *.xml
---
### Для работы програмы
0. Запуск программы.
1. Выбрать каталог в котором будут ожидаться файлы *.csv и *.xml
2. После того как прогармма обнаружит файлы, она будет искать совпадение записей по идентификатору пользователя
3. Если совпадение найдено, программа генерирует информацию о карте из двух источников (*.csv и *.xml)
4. В окне отображается количество сгенерированных карт, готовых к выводу в виде отчета (Рисунок 1)
5. Чтобы сгенерировать отчет, необходимо нажать кнопку "Сформировать отчёт".
6. В появившемся окне нужно указать каталог для сохранения имя файла и выбрать расширение файла (на данный момент поддерживается только *.xml) (Рисунок 2)

Пример входящего файла *.xml
~~~ xml
<?xml version="1.0" encoding="UTF-8" ?>
<Cards>
    <Card UserId="1">
        <Pan>1_23221312312312</Pan>
        <ExpDate>10/11/2017</ExpDate>
    </Card>
    <Card UserId="2">
        <Pan>2_3221312312312</Pan>
        <ExpDate>20/11/2017</ExpDate>
    </Card>
    <Card UserId="3">
        <Pan>3_221312312312</Pan>
        <ExpDate>30/11/2017</ExpDate>
    </Card>
        <Card UserId="4">
        <Pan>4_221312312312</Pan>
        <ExpDate>24/11/2017</ExpDate>
    </Card>
</Cards>
~~~
---
Пример входящего файла *.xml
~~~
1;Андрей;Андреев;1_9056938119
2;Андрей;Андреев;2_9056938119
3;Андрей;Андреев;3_9056938119
4;Андрей;Андреев;4_9056938119
~~~
#### Рисунок 1
![Рисунок 1. Программа нашла сопоставление](http://joxi.ru/krDJ7bEH0Jwb32.jpg)

#### Рисунок 2
![рисунок2. Программа нашла сопоставление](http://joxi.ru/vAWbVdxckqG8L2.jpg)


