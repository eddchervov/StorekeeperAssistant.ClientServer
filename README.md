Стек технологий:
- backend: ASP.NET Core 3.1
- Web Api + SPA
- db: mssql
- orm: Entity Framework Core 3.0
- frontend: Vue.js

<b>«Помощник кладовщика»</b><br><br>
Данное приложение предназначено для перемещение товаров между складами компании, получение на склад извне, расход со склада. <br><br>
<b>Что имеем?</b><br>
В системе должны быть следующие справочники: «Склады компании» и «Номенклатуры», достаточно полей Id и Name. <br>
Перемещаемый товар(или ТМЦ) это совокупность номенклатуры и кол-ва. <br>
В одном перемещении между складами(приходе/расходе) может быть несколько ТМЦ, при этом номенклатуры не должны повторяться.<br><br>
<b>Что должно уметь?</b><br>
1)	Реализовать страницу со списком всех перемещений(время, откуда, куда, кнопка «удалить»)
2)	Форму создания перемещения, прихода/расхода (откуда, куда, список перемещаемых ТМЦ) 
3)	Отчет по остаткам с выбором склада и времени, на которое отображать остатки.
4)	Заполнять базу при инициализации(не менее 3-х складов и не менее 7-ми номенклатур).
