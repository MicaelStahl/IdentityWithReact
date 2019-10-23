import React from "react";

const date = new Date();

const year = date.getFullYear();

const Footer = props => {
  return (
    <div className="footer">
      <div className="border-top shadow fixed-bottom">
        <div className="ml-3">&copy; - {year} - Micael Ståhl</div>
      </div>
    </div>
  );
};

export default Footer;
