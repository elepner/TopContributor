import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'commits-review',
    template: require('./commits.component.html')
})
export class UsersComponent {

    constructor(http: Http) {
        http.get('/api/Commits?days=30').subscribe(result => {
        });
    }
}