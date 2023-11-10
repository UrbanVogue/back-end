###################
# BUILD FOR LOCAL DEVELOPMENT
###################

# Use an official Node.js runtime as a parent image
FROM node:21-alpine3.17 As development

# Set the working directory in the Docker image
WORKDIR /usr/src/app

# Copy package.json and package-lock.json to the working directory in the Docker image
COPY --chown=node:node package*.json ./

# Install any needed packages specified in package.json
RUN npm ci

# Copy the current directory contents into the Docker image
COPY --chown=node:node . .

# Use the node user from the image (instead of the root user)
USER node


###################
# BUILD FOR PRODUCTION
###################

FROM node:21-alpine3.17 As production

# Set the working directory in the Docker image
WORKDIR /usr/src/app

# Copy package.json and package-lock.json to the working directory in the Docker image
COPY --chown=node:node package*.json ./

# Install any needed packages specified in package.json
RUN npm ci

# Copy the current directory contents into the Docker image
COPY --chown=node:node . .

RUN npm run build

# Use the node user from the image (instead of the root user)
USER node

CMD ["node", "dist/main"]