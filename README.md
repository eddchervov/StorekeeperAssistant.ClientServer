<b>«Помощник кладовщика»</b><br><br>

<a target="_blank" href="http://sa.eddcher.ru/">DEMO</a>

Данное приложение предназначено для перемещение товаров между складами компании, получение на склад извне, расход со склада. <br><br>
<p class="card-subtitle mb-1">
  <b>Что имеется в системе.</b>
</p>
<ul>
  <li>В системе существуют следующие справочники: «Склады компании» и «Номенклатуры».</li>
  <li>Перемещаемый товар(или ТМЦ) это совокупность номенклатуры и кол-ва.</li>
  <li>В одном перемещении между складами(приходе/расходе) может быть несколько ТМЦ, при этом номенклатуры не повторяются.</li>
  <li>Страница со списком всех перемещений(время, откуда, куда, кнопка «удалить»)</li>
  <li>Страница создания перемещения, приход/расход (откуда, куда, список перемещаемых ТМЦ)</li>
  <li>Страница - отчет по остаткам с выбором склада и времени, на которое отображать остатки.</li>
  <li>Приложение умеет инициализировать базу при первом запуске (не менее 3-х складов и не менее 7-ми номенклатур).</li>
</ul>

<br>

<p class="card-subtitle mb-1">
  <b>Стек технологий.</b>
</p>
<ul>
  <li>ASP.NET Core 3.1.</li>
  <li><b>Server: </b> Web Api (3 уровневая архитектура - Bl, Dal, WebApi).</li>
  <li><b>Client: </b> WebApp MVC + Webpack + Vue 2 + Vuex + доп. библиотеки
    (<a target="_blank" href="https://github.com/sagalbot/vue-select">vue-select</a>, 
    <a target="_blank" href="https://github.com/chronotruck/vue-ctk-date-time-picker">vue-ctk-date-time-picker</a>,
    <a target="_blank" href="https://github.com/lokyoung/vuejs-paginate">vuejs-paginate</a>).
  </li>
  <li><b>Client: </b>WebApp MVC общается с <b>Server: </b> WebApi через Api библиотеку.</li>
  <li><b>База данных: </b> MsSql</li>
  <li><b>ORM: </b> Entity Framework Core 3</li>
  <li>NUnit тесты</li>
</ul>
