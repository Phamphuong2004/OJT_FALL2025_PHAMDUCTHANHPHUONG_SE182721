import React, { useEffect, useState } from "react";

export default function Clock({
  locale,
  showSeconds = false,
  showTZ = false,
  className = "",
}) {
  const [now, setNow] = useState(() => new Date());

  useEffect(() => {
    // update every second so seconds (if shown) remain accurate
    const t = setInterval(() => setNow(new Date()), 1000);
    return () => clearInterval(t);
  }, []);

  const loc =
    locale || (typeof navigator !== "undefined" ? navigator.language : "en-US");

  const timeOptions = { hour: "2-digit", minute: "2-digit" };
  if (showSeconds) timeOptions.second = "2-digit";
  if (showTZ) timeOptions.timeZoneName = "short";

  const time = new Intl.DateTimeFormat(loc, timeOptions).format(now);
  const datePart = new Intl.DateTimeFormat(loc, {
    weekday: "short",
    day: "numeric",
    month: "short",
  }).format(now);

  return (
    <div
      className={`site-clock ${className}`}
      aria-live="polite"
      title={now.toISOString()}
    >
      {time} &middot; {datePart}
    </div>
  );
}
