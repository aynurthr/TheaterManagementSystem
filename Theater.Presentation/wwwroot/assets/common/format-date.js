//to format the date as DD.MM.YYYY
export const formatDate = (date) => {
  const day = String(date.getDate()).padStart(2, "0");
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const year = date.getFullYear();
  return `${day}.${month}.${year}`;
};

//to format the date as "27 July 2024, 18:30"
export const formatLongDate = (date) => {
  const options = {
    day: "2-digit",
    month: "long",
    // year: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  };
  return date.toLocaleDateString("en-GB", options).replace(" at", ",");
};
