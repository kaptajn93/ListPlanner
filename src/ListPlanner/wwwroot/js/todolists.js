
function ToDoList(selected, name, items, userID, toDoListID) {
    var self = this;

    //create list
    self.selected = ko.observable(selected || false);
    self.name = ko.observable(name || '');
    self.items = ko.observableArray(items || []);
    self.user = ko.observable(userID || null);
    self.toDoListID = ko.observable(toDoListID || null)
    self.newItem = ko.observable(new Item());

    self.getCount = ko.computed(function () {
        var items = self.items();
        var count = 0;
        var getAllDone = ko.utils.arrayForEach(items, function (item) {
            if (item.isDone() === true) {
                count++
            }
        });
        return {
            'count': count,
            'total': self.items().length
        };
    });

    // New Item on list
    self.saveItem = function () {
        DontshowLast = false;
        var currentlistID = self.toDoListID;
        var data = ko.toJSON({
            ItemName: self.newItem().itemName,
            ToDoListID: currentlistID,
            IsDone: self.newItem().isDone,
        });
        $.ajax({
            method: "POST",
            url: "/listItems/Create",
            data: data,
            contentType: "application/json",
            dataType: "json",
        })
          .done(function (data, textStatus, jqXHR) {
              console.debug("success", data);
              self.newItem(new Item());
              var onReloadCallback = function () {
                  console.debug('onReloadCallback')
                  // self.selectList(currentlistID);
              }
              self.reloadListItems(onReloadCallback);

          })
          .fail(function (jqXHR, textStatus, errorThrown) {
              alert("error");
          })

    };
    self.addItemToSelectedList = function () {
        DontshowLast = false;
        var itemToBeAdded = self.newItem();
        if (itemToBeAdded.itemName().length <= '1') {
            alert('Name is required to be > 1');
            return false;
        }
        self.resetSelectedItem = function () {
            //self.errorMessage('');
            self.newItem(new Item());
        }
        self.resetSelectedItem();
    }

    //Remove item:                              url skal kunne ændres til listitems
    self.removeItem = function (listItem) {
        isDeleteList = false;
        $.ajax({
            method: "POST",
            url: "/todolists/DeleteItems/" + listItem.listItemID(),
            contentType: "application/json",
            dataType: "json",
        })
        .done(function (data, textStatus, jqXHR) {
            self.reloadListItems();                  
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            alert("error");
        })
    }
   
    // computed
    self.allDone = ko.computed(function () {

        var result = self.getCount();
        return result.count === result.total;
    });
    self.allDoneText = ko.computed(function () {
        var result = self.getCount();

        if (self.allDone() === true && result.total != 0) {
            return "All Done"
        }
        else if (result.total == 0) {
            return "Not started"
        }
        return ("Missing: " + (result.total - result.count) + " items")
    });
    self.cssStatusClass = ko.computed(function () {

        //$parent.selectedList() !== null && $parent.selectedList().name === name

        if (!self.allDone()) {
            return 'warning';
        }
        else if (self.allDone() === true && self.getCount().total == 0) {
            return "danger"
        }
        else {
            return 'success';

        }
    });
    self.itemCount = ko.computed(function () {
        return self.items().length;
    });

    self.reloadListItems = function () {
        
        $.getJSON("/todolists/todoJson")
                .done(function (todoLists) {

                    self.items([]);
                    var currentListID = self.toDoListID;
                    var currentList = ko.utils.arrayFirst(todoLists, function (list) {
                        return (currentListID() === list.toDoListID);
                    });
                    var mappedData = ko.utils.arrayMap(currentList.items, function (item) {
                        return new Item(item.itemName, item.isDone, item.toDoListID, item.listItemID);
                    });
                    self.items(mappedData);
            });
            //var x = currentList.getCount();
            //return todoList;
       // });
    }
    //self.reloadListItems();
}