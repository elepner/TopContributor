export interface Commit {
    id: string,
    authorId: number,
    author: User,
    created: number,
    projectId: string,
    deletions: number,
    insertions: number,
    message: string,
    commiterId: string,
}

export interface User {
    id: number,
    firstName: string,
    lastName: string,
    fullName: string,
    primaryEmail: string,
    accounts: string,
}

export interface RepoAccount {
    sourceRepoId: string,
    accountId: string,
    personId: number,
    user: User,
    email: string,
    name: string,
}