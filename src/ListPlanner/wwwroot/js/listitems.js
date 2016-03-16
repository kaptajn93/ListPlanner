function Item(itemName, isDone, toDoListID, listItemID) {
    var self = this;
    self.itemName = ko.observable(itemName);
    self.isDone = ko.observable(isDone || false);
    self.toDoListID = ko.observable(toDoListID || null)
    self.listItemID = ko.observable(listItemID || null)

//    update item with isDone:
    self.isDone.subscribe(function (val) {
        self.updateItem();
    });

    self.updateItem = function () {
        DontshowLast = false;
        var data = ko.toJSON({
            ItemName: self.itemName(),
            ToDoListID: self.toDoListID(),
            IsDone: self.isDone(),
            ListItemID: self.listItemID(),
        });
        $.ajax({
            method: "POST",
            url: "/ListItems/Update/",
            data: data,
            contentType: "application/json",
            dataType: "json",
        })
      .done(function (data, textStatus, jqXHR) {
          var onReloadCallback = function () {
              console.debug('onReloadCallback')
       //       self.selectList(currentlistID);
          }
       //   self.reload(onReloadCallback);
      })
      .fail(function (jqXHR, textStatus, errorThrown) {
          alert("error");
      })
    }
   
    self.thisItem = ko.computed(function () {
        return self.itemName() + "," + self.isDone() + "," + self.toDoListID() + "," + self.listItemID();
    })
}