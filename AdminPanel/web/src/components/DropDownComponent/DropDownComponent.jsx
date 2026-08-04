import React, { useState } from "react";
import { AiFillCaretUp, AiFillCaretDown } from "react-icons/ai";

import styles from "./styles.module.css";

const DropDownComponent = ({ title, items, value, setValue, customstyle }) => {
  const [dropwDown, setDropDown] = useState(false);

  return (
    <div className={styles.wrapper}>
      <p className={styles.title}>{title}</p>
      <div className={styles.box} onClick={() => setDropDown((prev) => !prev)}>
        {dropwDown ? (
          <AiFillCaretUp className={styles.arrow} />
        ) : (
          <AiFillCaretDown className={styles.arrow} />
        )}
        <p className={`${styles.value} ${styles.items}`}>{value}</p>
      </div>
      {dropwDown && (
        <div className={styles.dropDowns} style={customstyle === true ? {height: '200px', overflow: 'auto'} : {}}>
          {items.map((el, i) => (
            <p
              className={styles.items}
              key={i}
              onClick={() => {
                setDropDown(false);
                setValue(el);
              }}
            >
              {el}{" "}
            </p>
          ))}
        </div>
      )}
    </div>
  );
};

export default DropDownComponent;

// import React, { useState } from "react";
// import { BiChevronDown, BiChevronUp } from "react-icons/bi";
// import styles from "./styles.module.css";

// const DropDownComponent = ({ title, items, value, setValue, index }) => {
//   const [active, setActive] = useState(false);
//   return (
//     <div className={styles.wrapper}>
//       <div className={styles.box} onClick={() => setActive((prev) => !prev)}>
//         <p className={styles.title}>{title}</p>
//         <BiChevronDown className={styles.arrow} />
//       </div>
//       {active && (
//         <div className={styles.dropDowns}>
//           {items.map((el, i) => (
//             <p className={styles.items} key={i}>
//               {el}{" "}
//             </p>
//           ))}
//         </div>
//       )}
//     </div>
//   );
// };

// export default DropDownComponent;
