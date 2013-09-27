App.TeamsRoute = Ember.Route.extend({
    model: function () {
        return this.store.findAll('team');
    }
});