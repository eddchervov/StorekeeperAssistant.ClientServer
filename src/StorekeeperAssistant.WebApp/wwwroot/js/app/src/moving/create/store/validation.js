
export function validationMoving({ departureWarehouseInventoryItems }) {

    var countError = 0;
    var messages = [];

    departureWarehouseInventoryItems.forEach(function (d) {
        if (d.newCount) {
            if (d.count < d.newCount) {
                countError++;
                messages.push(d.inventoryItem.name + ": Вы ввели значения больше чем находится на складе");
            }
        }
    });

    return { isError: countError != 0, countError: countError, messages: messages };
}
