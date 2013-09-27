App.UsersRoute = Ember.Route.extend({
    model: function () {
        //return this.store.findAll('user');
    },

    setupController: function(controller) {
        controller.set('content', this.store.findAll('user'));
    }
});