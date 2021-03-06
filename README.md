# XML-sync
Результат виконання тестового завдання XML-sync

### Завдання
Необхідно створити діалогову аплікацію, яка буде візуалізувати, аналізувати та синхронізувати по заданим параметрам XML-дерева. 

### Реалізація
Програму розроблено під операційну систему Windows за допомогою мови програмування C# з використанням технології WPF. За допомогою цієї програми можна відкрити два XML-файли, порівняти їх та зберегти результат синхронізації за певними параметратми у інший файл.
Оскільки відповідні вузли повинні знаходитись на відповідному рівні дерева та мати однакові імена тегу, для коректної роботи програми дочірні вузли одного вузла мають мати унікальні імена тегів.

### Інструкція користувача
Після запуску програми на екрані з'явиться вікно, в якому необхідно відкрити два файли за допомогою кнопок "Open left" та "Open right". Після цього, для порівняння та візувалізації дерева необхідно натиснути кнопку "Compare".
Для налаштування параметрів синхронізації необхідно натиснути кнопку "Sync". Після її натиснення на екрані з'явиться інше вікно, де можна змінити окремі параметри синхронізації.

##### Параметри:
- From left - додавати в результат вузли, які відсутні в правому документі але присутні в лівому;
- From right - додавати в результат вузли, які відсутні в лівому документі але присутні в правому;
- Unequal - додавати вузли, які відрізняються (якщо наступне поле має значення 'From left', то значення цих вузлів буде отримано з лівого документу)

Для збереженнярезультату необхідно натиснути кнопку "Sync".

### Результат викоання синхронізації

Лівий вхідний файл:
```xml
<root>
  <equal>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </equal>
  <unequal first="2">
    <first first="2" second="2">
      4
    </first>
    <second first="1">
      3
    </second>
  </unequal>
  <FromLeft>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </FromLeft>
</root>
```

Правий вхідний файл:
```xml
<root>
  <equal>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </equal>
  <unequal first="1" fromright="2">
    <first first="1" second="2">
      1
    </first>
    <second first="1">
      1
    </second>
  </unequal>
  <FromRight>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </FromRight>
</root>
```

Результат виконання синхронізації(з параметрами: ~~From right~~; ~~From left~~; ~~Unequal~~)
```xml
<root>
  <equal>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </equal>
  <FromLeft />
  <FromRight />
</root>
```


Результат виконання синхронізації(з параметрами: From right; ~~From left~~; Unequal From left)
```xml
<root>
  <equal>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </equal>
  <unequal first="2">
    <first first="2" second="2">
      4
    </first>
    <second first="1">
      3
    </second>
  </unequal>
  <FromRight>
    <first first="1" second="2">
      1
    </first>
    <second first="1" second="2">
      1
    </second>
  </FromRight>
</root>
```
