import React from "react";
// import { connect } from "react-redux";
import { Switch, Route } from "react-router-dom";

import Home from "./Home";
import Person from "../Products/Person/Person";
import ErrorPage from "./ErrorPage";
import Edit from "../Products/Person/Edit";
import Details from "../Products/Person/Details";
import Delete from "../Products/Person/Delete";

/**
 * The main body containing all links.
 */
const Main = () => {
  return (
    <React.Fragment>
      <Switch>
        <Route exact path="/Person" component={Person} />
        <Route exact path="/Person/Edit/:id" component={Edit} />
        <Route exact path="/Person/Details/:id" component={Details} />
        <Route exact path="/Person/Delete/:id" component={Delete} />

        <Route exact path="/" component={Home} />

        <Route exact component={ErrorPage} />
      </Switch>
    </React.Fragment>
  );
};

export default Main;
