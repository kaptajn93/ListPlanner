
var DontshowLast = false;
// showModal: myObservable 
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

        if (ko.utils.unwrapObservable(value) && DontshowLast === false) {
            modalElement.modal('show');
            // this is to focus input field inside dialog
            //$("input", modalElement).focus();
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

//function closeCurrentItems() {
//    document.getElementById("currentList").style.display = "none";
//}

function ViewModel(lists) {

    //observables:
    var self = this;
    self.toDoLists = ko.observableArray(lists || []);
    self.users = ko.observableArray([]);
    self.selectedUser = ko.observable(null);
    self.selectedList = ko.observable(null).logIt('selectedList');
    self.newToDoList = ko.observable(new ToDoList());

    //selectedlist functions:
    self.selectList = function (list) {
        DontshowLast = false;
        var listToSelect = list;
        if (typeof (listToSelect) !== 'object') {
            listToSelect = self.getListById(list);
        }
        self.selectedList(listToSelect);
    } 
    self.getListById = function (listID) {
        var list = ko.utils.arrayFirst(self.toDoLists(), function (list, index) {
            if (list.toDoListID() === listID) {
                return list;
            }
        });
        return list;
    };
    self.showList = ko.computed(function () {
        return self.selectedList != null;
    });
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
                if (currentItem.isDone === true) {
                    numDone++;
                }
            }
            return numDone;
        }
    });

    //New ToDoList
    self.addToDoList = function () {
        DontshowLast = true;
        var listToBeAdded = self.newToDoList();
        if (listToBeAdded.name().length <= '1') {
            alert('Name is required to be > 1');
            return false;
        }
        self.toDoLists.push(listToBeAdded);
        self.resetList();
    };
    self.resetList = function () {
        //self.errorMessage('');
        self.newToDoList(new ToDoList());

    };
    self.saveList = function () {
        DontshowLast = true;
        var data = ko.toJSON({
            Title: self.newToDoList().name,
            UserID: self.newToDoList().user().userID,
            Selected: self.newToDoList().selected,
        });

        $.ajax({
            method: "POST",
            url: "/todolists/Create",
            data: data,
            contentType: "application/json",
            dataType: "json",
        })
          .done(function (data, textStatus, jqXHR) {
              self.reload();
              self.newToDoList(new ToDoList());
          })
          .fail(function (jqXHR, textStatus, errorThrown) {
              alert("error");
          })
    };

    //Delete ToDoList
    self.removeToDoList = function (toDoList) {
        DontshowLast = true;
        $.ajax({
            method: "POST",
            url: "/todolists/Delete/" + toDoList.toDoListID(),
            contentType: "application/json",
            dataType: "json",
            //headers: {
            //    'RequestVerificationToken': '@TokenHeaderValue()'
            //}
        })
          .done(function (data, textStatus, jqXHR) {
              //alert("success");
                     self.reload();                              
              //self.toDoLists.remove(toDoList);
          })
          .fail(function (jqXHR, textStatus, errorThrown) {
              alert("error");
          })
          .always(function (data, textStatus) {
              //  alert("complete");
          });
    }

    //load data into site (from controller)
    self.reload = function (callback) {
        console.debug('reload')

        self.toDoLists([]);
        // Send an AJAX request Todolist
        $.getJSON("/todolists/toDoJson")
            .done(function (data) {
                console.debug('todolists result', data);

                // On success, 'data' contains a list of products.
                var lists = $.map(data, function (item) {
                    var items = $.map(item.items, function (item) {
                        return new Item(item.itemName, item.isDone, item.toDoListID, item.listItemID)
                    });
                    var todoList = new ToDoList(item.selected, item.title, items, item.userID, item.toDoListID)
                    var x = todoList.getCount();
                    return todoList;
                });
                self.toDoLists(lists);
                if ($.isFunction(callback) && DontshowLast === false) {
                    callback(lists);
                }
            });
        // Send an AJAX request User
        $.getJSON("/users/list")
            .done(function (data) {
                console.debug('user data', data);
                self.users(data);
            });
    }
    self.reload();
}
var vm = new ViewModel();
ko.applyBindings(vm);