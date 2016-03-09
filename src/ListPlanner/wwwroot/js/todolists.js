
function ToDoList(selected, name, items, userID) {
    var self = this;

    //create list
    self.selected = ko.observable(selected || false);
    self.name = ko.observable(name || '');
    self.items = ko.observableArray(items || []);

    self.user = ko.observable(userID || null);

    self.newItem = ko.observable(new Item());

    self.addItem = function () {

        var itemToBeAdded = self.newItem();

        if (itemToBeAdded.itemName().length <= '1') {
            alert('Name is required to be > 1');
            return false;
        }
        self.items.push(itemToBeAdded);
        self.resetItem();
    }


    //delete item on list
    self.removeItem = function (item) {
        self.items.remove(item);
    }

    //dont know....
    self.resetItem = function () {
        //self.errorMessage('');
        self.newItem(new Item());
    }

    //Update items on list

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
        else if(result.total == 0){
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
    //self.description = ko.computed(function () {
    //    return self.name() + "," + self.itemCount();
    //})
}