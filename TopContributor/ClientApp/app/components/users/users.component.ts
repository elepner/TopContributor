import { Component } from '@angular/core';
import { Http } from '@angular/http';
import Model = require("../../model/model");

@Component({
    selector: 'commits-review',
    template: require('./users.component.html')
})
export class UsersComponent {
    users: Model.User[];
    constructor(http: Http) {
        http.get('/api/Users').subscribe(result => {
            this.users =result.json();
        });
    }
}