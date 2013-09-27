App.DropdownPickerComponent = Ember.Component.extend(Ember.DropDown, {
    classNames: ['picker'],

    content: null,
    selection: null,
    text: null,

    multiple: false,

    query: '',
    minimumCharacters: 0,

    showList: function () {
        return this.get('isOpen') && this.get('query').length >= this.get('minimumCharacters');
    }.property('isOpen', 'query'),

    open: function () {
        this.$('.picker-dropdown').css({
            left: 0,
            top: this.$('.btn-toggle').outerHeight()
        });
        this.$('.picker-dropdown').show();
        this.$('.picker-query').focus();
    },

    close: function () {
        this.$('.picker-dropdown').hide();
    },

    toggleItem: function (item) {
        var replace = Ember.EnumerableUtils.replace,
            selection = this.get('selection'),
            multiple = this.get('multiple'),
            isArray = Ember.isArray(selection),
            newSelection;

        if (multiple) {
            if (isArray) {
                if (Ember.isNone(item)) {
                    newSelection = Em.A([]);
                } else if (selection.contains(item)) {
                    newSelection = selection.without(item);
                } else {
                    newSelection = Ember.A(selection.toArray());
                    newSelection.push(item);
                }
                replace(selection, 0, selection.get('length'), newSelection);
                this.notifyPropertyChange('selection');
            } else {
                this.set('selection', Ember.A([item]).compact());
            }
            this.$('.picker-query').focus();
        } else {
            this.set('selection', item);
            this.set('isOpen', false);
        }
    },

    clearSelection: function () {
        this.toggleItem();
    },

    emptyLabel: function () {
        return this.get('multiple') ? '0' : 'none';
    }.property('multiple'),

    selectionLabel: function () {
        var textProperty = this.get('text') || 'text';

        if (this.get('multiple')) {
            if (this.get('selection.length') == 1) {
                return this.get('selection').objectAt(0).get(textProperty);
            }
            return this.get('selection.length') || this.get('emptyLabel');
        } else {
            if (!this.get('selection')) {
                return this.get('emptyLabel');
            }
            return this.get('selection').get(textProperty);
        }
    }.property('selection', 'multiple'),

    filteredContent: function () {
        var query = this.get('query');
        if (!query)
            return this.get('content');

        var textProperty = this.get('text') || 'text';
        return this.get('content').filter(function (item) {
            return item.get(textProperty).toLowerCase().indexOf(query.toLowerCase()) > -1;
        });
    }.property('content.@each', 'query'),

    getIndexByElement: function (element) {
        return this.$('li').index(element);
    },

    activeIndex: -1,

    didQueryChanged: function () {
        this.set('activeIndex', -1);
    }.observes('query'),

    mouseMove: function (e) {
        var index = this.getIndexByElement($(e.target));

        if (this.get('clientX') == e.clientX && this.get('clientY') == e.clientY)
            return;

        if (index > -1) {
            this.set('activeIndex', index);
        }

        this.set('clientX', e.clientX);
        this.set('clientY', e.clientY);
    },

    keyDown: function (e) {
        if (e.keyCode == 13) {
            var item = this.get('filteredContent').objectAt(this.get('activeIndex'));
            this.toggleItem(item);
        }
        if (e.keyCode == 27) {
            this.clearSelection();
        }
        if (e.keyCode == 38) { // UP
            var activeIndex = this.get('activeIndex');
            if (activeIndex > 0) {
                this.set('activeIndex', activeIndex - 1);
                this.scrollIntoView();
            }
        }
        if (e.keyCode == 40) { // DOWN
            var activeIndex = this.get('activeIndex'),
                length = this.get('filteredContent.length');
            if (activeIndex < length - 1) {
                this.set('activeIndex', activeIndex + 1);
                this.scrollIntoView();
            }
        }
    },

    scrollIntoView: function () {
        var container = this.$('.picker-dropdown-body');
        var element = $(container).find('li').get(this.get('activeIndex'));
        var containerTop = $(container).scrollTop();
        var containerBottom = containerTop + $(container).height();
        var elemTop = element.offsetTop;
        var elemBottom = elemTop + $(element).height();
        if (elemTop < containerTop) {
            $(container).scrollTop(elemTop);
        } else if (elemBottom > containerBottom) {
            $(container).scrollTop(elemBottom - $(container).height());
        }
    },

    listView: Ember.CollectionView.extend({
        classNames: ['unstyled', 'picker-list'],
        tagName: 'ul',
        itemViewClass: Ember.View.extend({
            classNameBindings: ['active'],

            text: function () {
                var textProperty = this.get('controller.text');
                if (textProperty && textProperty.length) {
                    return this.get('content').get(textProperty);
                }
                return this.get('content').get('text');
            }.property('controller.text'),

            selected: function () {
                var content = this.get('content'),
                    selection = this.get('controller.selection');

                if (this.get('controller.multiple')) {
                    return selection && Ember.EnumerableUtils.indexOf(selection, content.valueOf()) > -1;
                } else {
                    return content == selection;
                }
            }.property('content', 'controller.selection'),

            active: function () {
                return this.get('contentIndex') == this.get('controller.activeIndex');
            }.property('controller.activeIndex'),

            init: function () {
                this._super();
                this.on('click', this, function () {
                    this.get('controller').toggleItem(this.get('content'));
                });
            },

            didItemChanged: function () {
                this.rerender();
            }.observes('content.text', 'selected'),

            render: function (buffer) {
                var escape = Handlebars.Utils.escapeExpression;
                if (this.get('selected')) {
                    buffer.push('<span>&#10004;</span>');
                } else {
                    buffer.push('<span>&nbsp;</span>');
                }
                buffer.push(escape(this.get('text')));
            }
        }),

        emptyView: Ember.View.extend({
            template: Ember.Handlebars.compile('<em class="muted">Your query is too thin.</em>')
        })
    })
});