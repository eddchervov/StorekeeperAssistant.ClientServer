
export function validationMoving({ departureWarehouseInventoryItems }) {

    var countError = 0;
    var messages = [];
    let isNotEmpty = false;
    departureWarehouseInventoryItems.forEach(function (d) {
        if (d.newCount) {
            isNotEmpty = true;
            if (d.count < d.newCount) {
                countError++;
                messages.push(d.inventoryItem.name + ": Вы ввели значения больше чем находится на складе");
            }
        }
    });

    if (isNotEmpty == false) {
        countError++;
        messages.push('Нет добавленных номенклатур');
    }

    return { isError: countError != 0, countError: countError, messages: messages };
}

export function validationComing(vuex, { inventoryItems }) {

    var countError = 0;
    var messages = [];

    if (inventoryItems.length == 0) {
        countError++;
        messages.push('Нет добавленных номенклатур');
    }
    inventoryItems.forEach(function (d) {
        if (d.newCount > vuex.$store.state.maxValueInventoryItem) {
            countError++;
            messages.push(d.name + " - максимально возможное значение: " + vuex.$store.state.maxValueInventoryItem);
        }
        else if (d.newCount < 0) {
            countError++;
            messages.push(d.name + " - минимально возможное значение: " + 1);
        }
    });

    return { isError: countError != 0, countError: countError, messages: messages };
}
