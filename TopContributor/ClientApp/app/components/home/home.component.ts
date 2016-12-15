import { Component } from '@angular/core';
import { Http } from '@angular/http';
import Model = require("../../model/model");

@Component({
    selector: 'home',
    template: require('./home.component.html')
})
export class HomeComponent {

    top7: Model.UserStat[];
    top30: Model.UserStat[];
    top: Model.UserStat[];
    commitsCount: number;

    constructor(http: Http) {
        http.get('/api/Users/top?days=7&limit=10').subscribe(result => {
            this.top7 = result.json();
        });

        http.get('/api/Users/top?days=30&limit=10').subscribe(result => {
            this.top30 = result.json();
        });

        http.get('/api/Users/top?days=5000&limit=10').subscribe(result => {
            this.top = result.json();
        });

        http.get('/api/Commits/stats').subscribe(result => {
            this.commitsCount = result.json();
        });
    }

}
