# Case module

Case lifecycle for all 3 types (AVR, Forced, VoluntaryOwnMeans): stages,
statuses, approval items, generated documents. Calls Rules module in-process
at every stage transition attempt.

Planned: CaseService, CreateCaseCommand, TransitionStageCommand,
ApprovalItemDto. Schema: case.
