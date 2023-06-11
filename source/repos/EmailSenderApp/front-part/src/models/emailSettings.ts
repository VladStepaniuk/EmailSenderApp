export interface EmailSettings{
    intervalValue: number,
    startTime: number,
    endTime: number,
    isScheduled: boolean,
    periodicity: string,
    daysOfWeek: number[]
}