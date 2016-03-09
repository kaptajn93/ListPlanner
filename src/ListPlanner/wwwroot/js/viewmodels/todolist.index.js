

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
    self.selectedList = ko.observable(null);

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

        // Send an AJAX request
        $.getJSON("/todolists/toDoJson")
            .done(function (data) {
                // On success, 'data' contains a list of products.
                var lists = $.map(data, function (item) {
                    var items = $.map(item.items, function (item) {
                        return new Item(item.itemName, item.isDone)
                    });

                    var todoList = new ToDoList(item.selected, item.title, items, item.userID)
                    var x = todoList.getCount();
                    return todoList;
                });
               

                self.toDoLists(lists);
            });

        // Send an AJAX request
        $.getJSON("/users/list")
            .done(function (data) {


                console.debug('user data', data);

                self.users(data);
            });
    }

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