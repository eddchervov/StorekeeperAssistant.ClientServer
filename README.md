<b>«Помощник кладовщика»</b><br><br>

Данное приложение предназначено для перемещение товаров между складами компании, получение на склад извне, расход со склада. <br><br>
<b>Что имеем?</b><br><br>
В системе должны быть следующие справочники: «Склады компании» и «Номенклатуры», достаточно полей Id и Name. <br>
Перемещаемый товар(или ТМЦ) это совокупность номенклатуры и кол-ва. <br>
В одном перемещении между складами(приходе/расходе) может быть несколько ТМЦ, при этом номенклатуры не должны повторяться.<br><br>
<b>Что должно уметь?</b><br>
1)	Реализовать страницу со списком всех перемещений(время, откуда, куда, кнопка «удалить»)
2)	Форму создания перемещения, прихода/расхода (откуда, куда, список перемещаемых ТМЦ) 
3)	Отчет по остаткам с выбором склада и времени, на которое отображать остатки.
4)	Заполнять базу при инициализации(не менее 3-х складов и не менее 7-ми номенклатур).

<br>

Стек технологий:
- ASP.NET Core 3.1
- Server: Web Api (3 уровневая архитектура)
- Client: MVC + Vue.js
- Связь между проектами через Api
- Db: MsSql
- ORM: Entity Framework Core 3.0
