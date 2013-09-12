App.Serializer = DS.RESTSerializer.extend({

    extractHasMany: function (type, hash, key) {
        var valueType = Ember.typeOf(hash[key])

        if (loadType === 'string') {
            return { url: hash[key] };
        }

        return this._super(type, hash, key);
    }

});

App.Adapter = DS.RESTAdapter.extend({

    namespace: 'api',
    serializer: App.Serializer.create(),

    findHasMany: function (store, record, relationship, details) {
        // TODO Handle hasMany relation by url
    }

});

App.reopen({

    Store: DS.Store.extend({

        adapter: App.Adapter.create()

    })

});
