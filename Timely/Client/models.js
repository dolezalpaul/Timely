var attr = DS.attr;

DS.Model.reopen({
    createdAt: attr({ type: 'date' }),
    updatedAt: attr({ type: 'date' }),
    version: attr({ type: 'number' })
});

App.User = DS.Model.extend({
    name: attr({ type: 'string' }),
    email: attr({ type: 'string' })
});

App.Project = DS.Model.extend({
    name: attr({ type: 'string' })
});

App.Task = DS.Model.extend({
    name: attr({ type: 'string' })
});

App.Favorite = DS.Model.extend({
    user: DS.belongsTo('user'),
    project: DS.belongsTo('project'),
    task: DS.belongsTo('task')
});