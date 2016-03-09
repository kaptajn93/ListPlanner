function Item(itemName, isDone /*, ToDoListID*/) {
    var self = this;
    self.itemName = ko.observable(itemName);
    self.isDone = ko.observable(isDone || false);

//     self.ToDoListID = function(todolist){
//         return todolist.ToDoListID;
//};

    self.thisItem = ko.computed(function () {
        return self.itemName() + "," + self.isDone()// + "," + ToDoListID
    })
}