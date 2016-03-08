function Item(itemName, isDone) {
    var self = this;
    self.itemName = ko.observable(itemName);
    self.isDone = ko.observable(isDone || false);

    self.thisItem = ko.computed(function () {
        return self.itemName() + "," + self.isDone()
    })
}