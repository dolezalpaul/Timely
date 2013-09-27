Ember.DropDown = Ember.Mixin.create({
    closeOnClick: true,
    isOpen: false,

    didOpenChanged: function () {
        if (this.get('isOpen')) {
            this.open();
        } else {
            this.close();
        }
    }.observes('isOpen'),

    open: Ember.K,

    close: Ember.K,

    toggle: function () {
        this.toggleProperty('isOpen');
    }
});
