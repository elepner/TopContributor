import { Component } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'commits-review',
    template: require('./commits.component.html')
})
export class CommitsComponent {
    public commits: Commit[];

    constructor(http: Http) {
        http.get('/api/Commits?days=10').subscribe(result => {
            this.commits = result.json();
        });
    }
}

interface Commit {
    id: string,
    authorId: number,
    created: number,
    projectId: string,
    deletions: number,
    insertions: number,
    message: string,
    author: Author,
}

interface Author {
    id: number,
    fullName: string,
    primaryEmail: string
}
