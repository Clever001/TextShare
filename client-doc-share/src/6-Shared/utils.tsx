export function dateToString(d: Date): string {
  var day: string = d.getDate().toString();
  if (day.length == 1) {
    day = "0" + day;
  }

  var month: string = (d.getMonth() + 1).toString();
  if (month.length == 1) {
    month = "0" + month;
  }

  return `${day}.${month}.${d.getFullYear()}`;
};