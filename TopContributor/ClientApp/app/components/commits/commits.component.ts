import { Component } from '@angular/core';
import { Http } from '@angular/http';
import Model = require("../../model/model");

@Component({
    selector: 'commits-review',
    template: require('./commits.component.html')
})
export class CommitsComponent {
    public commits: Model.Commit[];

    constructor(http: Http) {
        http.get('/api/Commits?days=30').subscribe(result => {
            this.commits = result.json();
        });
    }
}

