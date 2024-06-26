export const truncateText = (description, maxChars = 200) => {
  const tempDiv = document.createElement("div");
  tempDiv.innerHTML = description;
  const text = tempDiv.innerText || tempDiv.textContent;

  if (text.length <= maxChars) return text;
  return text.substring(0, maxChars) + "...";
};
