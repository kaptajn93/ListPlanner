function Item(itemName, isDone , toDoListID, listItemID) {
    var self = this;
    self.itemName = ko.observable(itemName);
    self.isDone = ko.observable(isDone || false);

    self.toDoListID = ko.observable(toDoListID || null)
    self.listItemID = ko.observable(listItemID || null)

//     self.ToDoListID = function(todolist){
//         return todolist.ToDoListID;
//};
    self.thisItem = ko.computed(function () {
        return self.itemName() + "," + self.isDone() + "," + self.toDoListID() + "," + self.listItemID();
    })
}