App.Router.reopen({
    location: 'history'
});

App.Router.map(function () {
    this.route('index', { path: '/' });
    this.resource('teams', function () {
        this.resource('team', { path: '/:team_id' });
    });
    this.resource('users');
});
