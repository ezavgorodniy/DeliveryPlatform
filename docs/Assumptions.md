This page contains assumption which I made because of uncertainty about initial requirements

## Assumptions

- didn't find an information which exact role may create, read and delete. I'm assuming user and partner both may read and update (update with some restrictions), and only user may create and delete delivery;
- 1 minute delay between delivery actually expired and application marked as Expired is acceptable;
- there is no way to update address, recipient or access window for delivery (if it's not true code should be easy modifiable)

So all rules are rephrased to:
  - nobody can change delivery if it's finished
  - API is not allowing to set state as Expired or Created
  - by default delivery is marked as Started
  - only User may approve delivery
  - Partner (not User) may complete delivery from Approved
  - Partner or User may cancel delivery 
  - everythin else is forbiden


