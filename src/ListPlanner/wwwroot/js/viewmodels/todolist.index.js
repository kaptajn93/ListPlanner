
// bindinghandlers: 
ko.bindingHandlers.showModal = {
    init: function (element, valueAccessor, allBindingsAccessor, ViewModel, bindingContext) {

        var x = ViewModel;
        var test = function () {
            bindingContext.$parent.selectList(bindingContext.$data);
            console.debug('click');
        };


        $(element).bind('click', test);

        //var handler = bindingContext.$parent.selectList.bind(bindingContext.$data);
        //ko.bindingHandlers.click.init(element, handler, allBindingsAccessor, ViewModel, bindingContext);
    },




    update: function (element, valueAccessor) {
        
        var value = valueAccessor();
        var modalElement = $("#currentList");

        if (ko.utils.unwrapObservable(value)) {
            modalElement.modal('show');
            // this is to focus input field inside dialog
            $("input", modalElement).focus();
        }
        else {
            modalElement.modal('hide');
        }
    }
};

ko.subscribable.fn.logIt = function (name) {
    this.triggeredCount = 0;
    this.subscribe(function (newValue) {
        if (window.console && console.log) {
            console.log(++this.triggeredCount, name + " triggered with new value", newValue);
        }
    }, this);

    return this;
};
// 


function closeCurrentItems() {

    document.getElementById("currentList").style.display = "none";
}
function ViewModel(lists) {

    var self = this;
    self.toDoLists = ko.observableArray(lists || []);

    self.users = ko.observableArray([]);
    self.selectedUser = ko.observable(null);
   

    //Edit list and items on list:
    //selected list
    self.selectedList = ko.observable(null).logIt('selectedList');


    self.selectList = function (list) {
        self.selectedList(list);
    }

    self.removeItemFromSelectedList = function (item) {
        self.selectedList().removeItem(item);
    }

    self.newItemOnSelected = ko.observable(new Item());

    self.addItemToSelectedList = function () {

        var itemToBeAdded = self.newItemOnSelected();

        if (itemToBeAdded.itemName().length <= '1') {
            alert('Name is required to be > 1');
            return false;
        }
        self.selectedList().items.push(itemToBeAdded);
        self.resetSelectedItem = function () {
            //self.errorMessage('');
            self.newItemOnSelected(new Item());
        }
        self.resetSelectedItem();
    }

    self.totalDone = ko.computed(function () {
        var numDone = 0;
        if (self.selectedList() == null) {
            return numDone;
        }

        else {

            var x = ko.utils.unwrapObservable(self.selectedList().items);
            var y = self.selectedList().items();

            for (var i = 0; i < y.length; i++) {

                var currentList = y;
                var currentItem = currentList[i];
                if (currentItem.isDone() === true) {
                    numDone++;
                }
            }
            return numDone;
        }
    });

    //create list
    // temp list
    self.newToDoList = ko.observable(new ToDoList());

    self.addToDoList = function () {

        //self.NextToDoListID = function () {
        //    var nextTdlID = calldb.getNextToDoListID();
        //    Item.getNextToDoListID(nextTdlID);
        //    return nextTdlID;
        //}

        var listToBeAdded = self.newToDoList();

        if (listToBeAdded.name().length <= '1') {
            alert('Name is required to be > 1');
            return false;
        }

        self.toDoLists.push(listToBeAdded);
        self.resetList();
    }

    //remove list
    self.removeToDoList = function (toDoList) {
        self.toDoLists.remove(toDoList);
    }

    self.resetList = function () {
        //self.errorMessage('');
        self.newToDoList(new ToDoList());

    };

    //show  a selected list
    self.showList = ko.computed(function () {
        return self.selectedList() != null;
    });

    //load data into site (from controller)
    self.reload = function () {
        // Send an AJAX request Todolist
        $.getJSON("/todolists/toDoJson")
            .done(function (data) {
                // On success, 'data' contains a list of products.
                var lists = $.map(data, function (item) {
                    var items = $.map(item.items, function (item) {
                        return new Item(item.itemName, item.isDone, item.toDoListID)
                    });
                    var todoList = new ToDoList(item.selected, item.title, items, item.userID, item.toDoListID)
                    var x = todoList.getCount();
                    return todoList;
                });
                self.toDoLists(lists);
            });
        // Send an AJAX request User
        $.getJSON("/users/list")
            .done(function (data) {
                console.debug('user data', data);
                self.users(data);
            });
    }

    self.saveList = function () {
        var data = ko.toJSON({
            Title: self.newToDoList().name,
            UserID: self.newToDoList().user().userID,
            Selected: self.newToDoList().selected,
        });
                
        $.ajax( {
            method: "POST",
            url: "/todolists/Create",
            data: data ,
            contentType: "application/json",
            dataType: "json",
            //headers: {
            //    'RequestVerificationToken': '@TokenHeaderValue()'
            //}
        })
          .done(function(data, textStatus, jqXHR) {
              alert( "success" );
          })
          .fail(function( jqXHR, textStatus, errorThrown) {
              alert( "error" );
          })
          .always(function(data, textStatus) {
              alert( "complete" );
          });
    };

    self.saveItem = function () {
        var listID = self.selectedList().toDoListID();

        var data = ko.toJSON({
            ItemName: self.newItemOnSelected().itemName,
            ToDoListID: listID,
            IsDone: self.newItemOnSelected().isDone,
        });

        $.ajax({
            method: "POST",
            url: "/listItems/Create",
            data: data,
            contentType: "application/json",
            dataType: "json",
            //headers: {
            //    'RequestVerificationToken': '@TokenHeaderValue()'
            //}
        })
          .done(function (data, textStatus, jqXHR) {
              console.debug("success", data);
              
              //self.reload();
              var first = ko.utils.arrayFirst(vm.toDoLists(), function (list, index) {
                  if (list.toDoListID() === listID) return list;
              });

              var newItem = self.newItemOnSelected();
              self.selectedList().items.push(newItem);

              //self.selectList(null);
              //self.selectList(first);
          })
          .fail(function (jqXHR, textStatus, errorThrown) {
              alert("error");
          })
          .always(function (data, textStatus) {
              alert("complete");
          });
    };

// load initial data
self.reload();
    
}

//vm.toDoLists([
//    new ToDoList(false, "Sleep Over", [
//        new Item("Fine Wine", false),
//        new Item("Sleeping bag", false)]),
//    new ToDoList(false, "Party at 5th", [
//        new Item("Fine Wine", false),
//        new Item("Jelly shots", false)]),
//]);


var vm = new ViewModel();
ko.applyBindings(vm);