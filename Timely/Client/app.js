window.App = Ember.Application.create();

App.ApplicationView = Ember.View.extend({
    templateName: 'application',
    elementId: 'application-container',

    currentPathDidChange: function () {
        Ember.run.next(this, function () {
            this.$("ul.nav li:has(>a.active)").addClass('active');
            this.$("ul.nav li:not(:has(>a.active))").removeClass('active');
        });
    }.observes('controller.currentPath')
});
