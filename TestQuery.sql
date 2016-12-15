select * from Users left join RepositoryAccounts on Users.Id=RepositoryAccounts.PersonId 
left join Commits on 
RepositoryAccounts.AccountId=Commits.VSCAuthorAccountId AND RepositoryAccounts.SourceRepoId=Commits.VSCRepositoryId where Users.Id=41